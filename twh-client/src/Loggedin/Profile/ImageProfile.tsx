import React, { useState, useEffect } from "react";
import {
    Image,
    Avatar
} from "@fluentui/react-components";

import CustomUpload from "../Common/CustomUpload";
import Styles from "../../Styles/FluentStyles";

import { useUser } from "../Common/UserProvider";

const ImageProfile = (): React.ReactNode => {
    const styles = Styles();
    const { user, isLoggeding } = useUser();

    const [hasProfileImage, setHasProfileImage] = useState<boolean>(false);
    const [profileImagePath, setProfileImagePath] = useState<string>("");


    useEffect(() => {
        loadProfileImage();
    }, []);

    const loadProfileImage = () => {
        try {
            const uri = user?.uri;
            const path = require(`../../Assets/ProfileImages/${uri}.png`);
            setHasProfileImage(true);
            setProfileImagePath(path);
        } catch (error) {
            setHasProfileImage(false);
        }
    };

    const onProfileImageUpload = (file: any) => {
        
        console.log("== f ile: ", file);
        const reader = new FileReader();
        //const u = URL.createObjectURL(file);
        //  reader.readAsDataURL(new Blob(u));
        //console.log('==U: ', u, reader);
    }

    return (
        <CustomUpload fileType="image" onClickCallback={onProfileImageUpload} >
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
                    onClick={(file) => onProfileImageUpload(file)}
                    className={`${styles.profile_image}`}
                />
            ):
            (
                <Avatar
                    color="colorful"
                    name={`${user?.firstName} ${user?.lastName}`}
                    size={128}
                    onClick={(file) => onProfileImageUpload(file)}
                    className={`${styles.profile_image}`}
                />
            )
        }
        </CustomUpload>
    );
};

export default ImageProfile;