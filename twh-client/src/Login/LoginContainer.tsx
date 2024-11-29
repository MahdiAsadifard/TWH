
import React from "react";
import { Button, useId, Input, Label } from "@fluentui/react-components";
import { Mail24Regular, Key20Regular } from "@fluentui/react-icons";

import "./login.css";

interface IProps {
    callback?: ((e: boolean) => void);
};


// TODO: handle auto login

const LoginContainer: React.FunctionComponent<IProps> = ({
    callback
}): React.ReactElement => {

  const emailId = useId("input-email");
  const passwordId = useId("input-password");


    const onSignin = (e: any) => {
        if(callback) callback(true);
    };
    return (
        <div className="root">
            <form noValidate autoComplete="off"  className="fields">
                <div className="row">
                    <Label htmlFor={emailId} className="label">Email</Label>
                    <Input placeholder="johndoe@mail.com" type="email" id={emailId} contentAfter={<Mail24Regular />} />
                </div>
                <div className="row">
                    <Label htmlFor={passwordId} className="label">Password</Label>
                    <Input placeholder="Password" type="password" defaultValue="" id={passwordId} contentAfter={<Key20Regular />}  />
                </div>
                    <Button appearance="primary" onClick={onSignin}  >Signin</Button>
            </form>
        </div>
    );
};

export default LoginContainer;