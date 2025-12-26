import { lazy } from "react";
import { useNavigate } from "react-router";
import { paths } from "../../Routs/Router";

import * as Utils from "../../Global/Utils";

const Dialog = lazy(() => import('../Common/CustomDialog'));

/**
 * Signout component only returns a Dialog
 * @returns 
 */
export const Container = () => {
    const navigate = useNavigate();

    const onSignout = (e: any) => {
        Utils.DeleteAllCookies();
        navigate(paths.slash);
    };

    return (
        <Dialog title={`Signout`} content={`Are you sure you would like to signout?`}
                action = { { title: '', apperance: 'transparent' } }
                actions={
                    [
                        {
                            title: 'Yes',
                            apperance: 'primary',
                            onClick: (e) => onSignout(e),
                        },
                    ]
                }
                openFromOutside={true}
                closeAndBackHistory={true}
            />
    );
};
