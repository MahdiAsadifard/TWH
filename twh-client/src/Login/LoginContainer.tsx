import { Button } from "@fluentui/react-components";
import React from "react";

interface IProps {
    callback: (e: boolean) => void;
};

const LoginContainer: React.FunctionComponent<IProps> = ({
    callback
}): React.ReactElement => {
    const onSignin = (e: any) => {
        callback(true);
    };
    return (
        <>
            <Button appearance="primary" onClick={onSignin} >Signin</Button>
        </>
    );
};

export default LoginContainer;