
import React, { useState } from "react";
import { Button, useId, Input, Label } from "@fluentui/react-components";
import { Mail24Regular, Key20Regular } from "@fluentui/react-icons";

import { CheckLogin } from "../Mongo/Auth/AuthOperations";
import "./login.css";

interface IProps {
    callback?: ((e: boolean) => void);
};


// TODO: handle auto login

const LoginContainer: React.FunctionComponent<IProps> = ({
    callback
}): React.ReactElement => {

    const [email, setEmail] = useState<string>('');
    const [password, setPassword] = useState<string>('');

  const emailId = useId("input-email");
  const passwordId = useId("input-password");


    const onSignin = async (e: any) => {
        const submission = {
            Email: email,
            Password: password
        };
        console.log(submission);
        const response = await CheckLogin(submission);
        console.log("loginCOntainer response: ", response);
        // if(callback) callback(true);
    };
    return (
        <div className="root">
            <form noValidate autoComplete="off"  className="fields">
                <div className="row">
                    <Label htmlFor={emailId} className="label">Email</Label>
                    <Input placeholder="johndoe@mail.com" type="email" id={emailId} contentAfter={<Mail24Regular />} value={email} onChange={(e) => setEmail(e.target.value)} />
                </div>
                <div className="row">
                    <Label htmlFor={passwordId} className="label">Password</Label>
                    <Input placeholder="*****" type="password"  id={passwordId} contentAfter={<Key20Regular />} value={password} onChange={(e) => setPassword(e.target.value)}  />
                </div>
                    <Button appearance="primary" onClick={onSignin}  >Signin</Button>
            </form>
        </div>
    );
};

export default LoginContainer;