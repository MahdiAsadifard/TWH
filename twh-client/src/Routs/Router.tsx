import { lazy } from "react";
import { createBrowserRouter } from "react-router";

const ProfileContainer = lazy(() => import('../Loggedin/Profile/ProfileContainer'));
const LoginContainer = lazy(() => import("../Login/LoginContainer"));
const Main = lazy(()=> import( "../Main"));
const NotFound = lazy(() => import("../Loggedin/Common/NotFound"));
const DashboardContainer = lazy(() => import("../Loggedin/Dashboard/DashboardContainer"));
const SignedinContainer = lazy(() => import("../Loggedin/SignedinContainer"));
const Signout = lazy(() => import('../Loggedin/Signout/Index'));

export const paths = {
    slash: '/',
    login: '/login',
    signedin: '/in',
    dashboard: '/in/dashboard',
    profile: '/in/profile',
    signout: '/in/signout',
    all: '*'
};

export const router = createBrowserRouter([
    {
        path: paths.slash,
        element: <Main/>
    },
    {
        path: paths.signedin,
        element: <SignedinContainer />,
        children: [
            {
                path: paths.dashboard,
                element: <DashboardContainer />
            },
            {
                path: paths.profile,
                element: <ProfileContainer />
            },
            {
                path: paths.signout,
                element: <Signout />
            },
        ]
    },
     {
        path: paths.login,
        element: <LoginContainer />
    },
    {
        path: paths.all,
        element: <NotFound />
    }
]);