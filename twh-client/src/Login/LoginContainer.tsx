import { Button } from "@fluentui/react-components";
import React from "react";

interface IProps {
    callback?: ((e: boolean) => void);
};

// TODO: handle auto login

const LoginContainer: React.FunctionComponent<IProps> = ({
    callback
}): React.ReactElement => {

    const onSignin = (e: any) => {
        if(callback) callback(true);
    };
    return (
        <>
            <Button appearance="primary" onClick={onSignin} >Signin</Button>
        </>
    );
};

export default LoginContainer;