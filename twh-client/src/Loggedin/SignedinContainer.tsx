import { lazy, useEffect } from "react";

import { useUser } from "./Common/UserProvider";
const Nav = lazy(() => import('./Common/Nav'));

const SignedinContainer = () => {
    const { user, isLoggeding, clearUser } = useUser();
    console.log({
        user,
        isLoggeding
    })

    useEffect(() => {
       /// if(!isLoggeding) clearUser();
    }, []);

    return (
        <>
            <Nav />
        </>
    );
};

export default SignedinContainer;