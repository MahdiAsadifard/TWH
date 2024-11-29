import { lazy } from "react";
import { useNavigate } from "react-router";
import { paths } from "../../Routs/Router";

const Dialog = lazy(() => import('../Common/CustomDialog'));

/**
 * Signout component only returns a Dialog
 * @returns 
 */
export const Container = ()=>{
    const navigate = useNavigate();

    return (
        <Dialog title={`Signout`} content={`Are you sure you would like to signout?`}
                action = { { title: '', apperance: 'transparent' } }
                actions={
                    [
                        {
                            title: 'Yes',
                            apperance: 'primary',
                            onClick: (e) => {

                                console.log("Yes clicked!" + JSON.stringify(e));
                                navigate(paths.slash);
                            },
                        },
                    ]
                }
                openFromOutside={true}
                closeAndBackHistory={true}
            />
    );
};
