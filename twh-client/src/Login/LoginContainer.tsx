
import React, { lazy, useState } from "react";
import {
    makeStyles,
    Button,
    useId,
    Input,
    Label,
    Body1,
    Caption1,
    Checkbox
} from "@fluentui/react-components";
import { Mail24Regular, EyeOff20Filled, Eye20Regular } from "@fluentui/react-icons";

import {
    Card,
    CardHeader,
    CardFooter,
    CardPreview
} from "@fluentui/react-components";

import "../Styles/Login.css";
import useCustomToast from  "../Loggedin/Common/UseCustomToast";
import { StatusCode } from "../Global/RequestReponseHelper";
import useFetch from "../Loggedin/Common/UseFetch";
import Loading from "../Loggedin/Common/Loading";

const CustomButton = lazy(() => import("../Loggedin/Common/CustomButton"));

interface IProps {
    callback?: ((e: boolean) => void);
};


// TODO: handle auto login

const LoginContainer: React.FunctionComponent<IProps> = ({
    callback
}): React.ReactElement => {

    const styles = useStyles();
    const { showToast, ToasterComponent } = useCustomToast();
    const { error, loading, fetchResponse, ApiRequest } =  useFetch();

    const [email, setEmail] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const [showPassword, setShowPassword]=useState(false);
    const [rememberMe, setRememberMe]=useState(false);

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
        await ApiRequest({ url: `auth/login`, method: 'POST', body: submission, withToken: false  });

        if(error || !fetchResponse.success) {
            let title = 'Error', body = '';
            switch (fetchResponse.statusCode) {
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
                title,
                body
            });
        }else {
            const x = fetchResponse.response as any;
            const now = new Date();
            console.log(now);
            
            now.setSeconds(now.getSeconds() + x.data.token.expiry);
            console.log(now.toUTCString());
            document.cookie = `twh_token=${x.data.token?.accessToken};expires=${now.toUTCString()};domain=${window.location.hostname}`;
            if(callback) callback(true);
        }
    };

    if(loading) return <Loading FullScreen={true} />;
    return  (
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

                    <Checkbox
                        checked={rememberMe}
                        label={`Remember me`}
                        onChange={(e) => setRememberMe(e.target.checked)}
                        />
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