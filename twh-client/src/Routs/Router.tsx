import { createBrowserRouter } from "react-router";

import ProfileContainer from "../Loggedin/Profile/ProfileContainer";
import LoginContainer from "../Login/LoginContainer";
import Main from "../Main";
import NotFound from "../Loggedin/Common/NotFound";
import DashboardContainer from "../Loggedin/Dashboard/DashboardContainer";

export const paths = {
    slash: '/',
    login: '/login',
    dashboard: '/dashboard',
    profile: '/profile',
    all: '*'
};

export const router = createBrowserRouter([
    {
        path: paths.slash,
        element: <Main/>
    },
    {
        path: paths.login,
        element: <LoginContainer />
    },
    {
        path: paths.dashboard,
        element: <DashboardContainer />
    },
    {
        path: paths.profile,
        element: <ProfileContainer />
    },
    {
        path: paths.all,
        element: <NotFound />
    }
]);