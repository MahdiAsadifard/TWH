import { lazy } from "react";

const Nav = lazy(() => import('./Common/Nav'));

const SignedinContainer = () => {
    return (
        <>
            <Nav />
        </>
    );
};

export default SignedinContainer;