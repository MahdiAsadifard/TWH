
import React, { lazy, useState } from "react";
import {
    makeStyles,
    Button,
    useId,
    Input,
    Label,
    Body1,
    Caption1
} from "@fluentui/react-components";
import { Mail24Regular, EyeOff20Filled, Eye20Regular } from "@fluentui/react-icons";

import {
    Card,
    CardHeader,
    CardFooter,
    CardPreview
} from "@fluentui/react-components";


import "../Styles/Login.css";
import { CheckLogin } from "../Mongo/Auth/AuthOperations";
import UseCustomToast from  "../Loggedin/Common/UseCustomToast";
import { StatusCode } from "../Global/RequestReponseHelper";

const CustomButton = lazy(() => import("../Loggedin/Common/CustomButton"));

interface IProps {
    callback?: ((e: boolean) => void);
};


// TODO: handle auto login

const LoginContainer: React.FunctionComponent<IProps> = ({
    callback
}): React.ReactElement => {

    const styles = useStyles();
    const { showToast, ToasterComponent } = UseCustomToast();

    const [email, setEmail] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const [showPassword, setShowPassword]=useState(false);

    const emailId = useId("input-email");
    const passwordId = useId("input-password");

    const hideAndShowButton =
        <CustomButton
            shape="circular"
            icon={showPassword ? <Eye20Regular /> : <EyeOff20Filled /> }
            appearence="transparent"
            onClick={(e)=>{
                setShowPassword(prev => !prev);
            }}
        />;

    const onSignin = async (e: any) => {
        const submission = {
            Email: email,
            Password: password
        };
        const { success, statusCode, response } = await CheckLogin(submission);
        console.log("loginContainer response: ", response, statusCode, success);
        if(success) {
            if(callback) callback(true);
        }else {

            let title = 'Error', body = '';
            switch (statusCode) {
                case StatusCode.Unauthorized:
                    title = 'Unauthorized';
                    body = 'Invalid email or password, try again!';
                    break;
                case StatusCode.NotFound:
                    title = 'Not Fund';
                    body = 'User not found, please signup!';
                    break;
                    case StatusCode.BadRequest:
                    title = 'Bad Request';
                    body = 'Fill out email/password, try again!';
                    break;
                default:
                    body = 'Unknown error, please tryagain!'
                    break;
            }
           showToast({
                title: title,
                body: body,
            });
        }
    };

    return (
        <div className={` center full-height `}>
            <Card className={`card `} orientation="vertical">
                <CardHeader 
                    className=""
                    header={
                        <Body1>
                            <b>Sign in</b>
                            <div><Caption1>to continue to TWH</Caption1></div>
                        </Body1>
                    }
                />
                <CardPreview className={`preview ${styles.cardPreview}`}>
                     <span>
                        <span className="gap">
                            <Label htmlFor={emailId}>Email</Label>
                            <Input className="textbox" placeholder="johndoe@mail.com" type="email" id={emailId} contentAfter={<Mail24Regular />} value={email} onChange={(e) => setEmail(e.target.value)} />
                        </span>
                        <span className="gap">
                            <Label htmlFor={passwordId}>Password</Label>
                            <Input className={`textbox `} placeholder="*****" type={showPassword ? "text" : "password"}  id={passwordId} contentAfter={hideAndShowButton} value={password} onChange={(e) => setPassword(e.target.value)}  />
                        </span>
                    </span>
                </CardPreview>
                <CardFooter className="footer">
                    <Button className="" appearance="primary" onClick={onSignin}>Signin</Button>
                </CardFooter>
            </Card>
            <ToasterComponent />
        </div>
    );
};

const useStyles = makeStyles({
    cardPreview: {
        maxWidth: '50vw'
    }
});

export default LoginContainer;  