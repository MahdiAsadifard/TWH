import { useEffect, useState } from "react";
import {
    Image,
    Avatar
} from "@fluentui/react-components";

import { useUser } from "../Common/UserProvider";


const ProfileContainer = () => {
    const { user, isLoggeding } = useUser();
    const [hasProfileImage, setHasProfileImage] = useState<boolean>(false);
    const [profileImagePath, setProfileImagePath] = useState<string>("");


    useEffect(() => {
        loadProfileImage();
        console.log("ProfileContainer: ",user, isLoggeding)
    }, []);

    const loadProfileImage = () => {
        const uri = user?.uri;

        try {
            const path = require(`../../Assets/ProfileImages/${uri}.png`);
            setHasProfileImage(true);
            setProfileImagePath(path);
        } catch (error) {
            setHasProfileImage(false);
            
        }
    };

    return (
        <>
        {
            hasProfileImage ?
            (
                <Image 
                    alt="d1f2f_profile"
                    src={profileImagePath}
                    width={400}
                    height={400}
                    shape="circular"
                    fit="contain"
                />
            ):
            (
                <Avatar
                    color="colorful"
                    name="Mahdi Asadifard"
                    size={128}
                    // style={{ width: '300px', height: '300px' }}
                />
            )
        }
        </>
    );
};

export default ProfileContainer;