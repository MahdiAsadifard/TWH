
import { StatusCode } from "../Global/RequestReponseHelper"

export interface IUserReponseDto {
    uri: string;
    firstName: string;
    lastName: string;
    phone?: string;
    email: string;
    disabled: boolean;
};

export interface IToken {
    accessToken: string;
    accessTokenExpityUTC: Date;
    tokenType: string;
    refreshToken: string;
    refreshTokenExpityUTC: Date;
};

export interface ILoginReponse {
    isSuccess: boolean;
    message: string;
    statusCode: StatusCode;
    data: {
        user: IUserReponseDto;
        token: IToken;
    };
    rememberMe?: boolean; // indicates if the user opted for "remember me" during login
};