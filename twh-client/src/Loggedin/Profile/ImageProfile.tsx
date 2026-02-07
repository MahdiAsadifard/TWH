import React, { useState, useEffect, use } from "react";
import {
    Image,
    Avatar
} from "@fluentui/react-components";

import CustomUpload from "../Common/CustomUpload";
import Styles from "../../Styles/FluentStyles";
import UseFetch from "../Common/UseFetch";
import { useUser } from "../Common/UserProvider";
import Loading from "../Common/Loading";

const ImageProfile = (): React.ReactNode => {
    const { ApiRequest, fetchResponse, error, loading } = UseFetch();
    const styles = Styles();
    const { user, isLoggeding } = useUser();

    const [hasProfileImage, setHasProfileImage] = useState<boolean>(false);
    const [profileImagePath, setProfileImagePath] = useState<string>("");


    useEffect(() => {
        loadProfileImage();
    }, []);

    const loadProfileImage = async () => {
        try {
            await ApiRequest({
                url: `${user?.uri}/user/profileimage`,
                method: "GET",
                withToken: true,
                isUpload: true
            });
            if(!error || fetchResponse.success) {
                const contentBase64 = await fetchResponse.response.data.content;//Base64;
                const base64 = `data:${fetchResponse.response.data.contentType};base64,${contentBase64}`;
                setProfileImagePath(base64);
                setHasProfileImage(true);
            }
        } catch (error) {
            setHasProfileImage(false);
            console.log('Error on loading profile image: ', error);
        }
    };

    const onProfileImageUpload = async (file: any) => {
        const formData = new FormData();
        formData.append("file", file);

        await ApiRequest({
            url: `${user?.uri}/user/upload`,
            method: "POST",
            withToken: true,
            body: formData,
            isUpload: true
        });

        await loadProfileImage();
    }

    if(loading) return <Loading FullScreen={false} />;

    return (
        <CustomUpload fileType="image" onClickCallback={(file: HTMLFormElement) => onProfileImageUpload(file)} >
        {
            hasProfileImage ?
            (
                <Image 
                    alt={`${user?.uri}_profile`}
                    src={profileImagePath}
                    width={400}
                    height={400}
                    shape="circular"
                    fit="contain"
                    className={`${styles.profile_image}`}
                />
            ):
            (
                <Avatar
                    color="colorful"
                    name={`${user?.firstName} ${user?.lastName}`}
                    size={128}
                    className={`${styles.profile_image}`}
                />
            )
        }
        </CustomUpload>
    );
};

export default ImageProfile;