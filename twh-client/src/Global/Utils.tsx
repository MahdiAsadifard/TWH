import { ILoginReponse } from "../Login/Types";


export enum Cookies {
    token = 'twh_token',
    refreshToken = 'twh_refresh_token',
    customerUri = 'twh_customer_uri',
};

export const SetCookies = (response: ILoginReponse) => {
    const tokenExpiryDate = new Date(response.data.token.accessTokenExpityUTC);
    const refreshTokenExpiryDate = new Date(response.data.token.refreshTokenExpityUTC);

    document.cookie = `${Cookies.token}=${response.data.token?.accessToken};expires=${tokenExpiryDate.toUTCString()};domain=${window.location.hostname}`;
    document.cookie = `${Cookies.refreshToken}=${response.data.token?.refreshToken};expires=${refreshTokenExpiryDate.toUTCString()};domain=${window.location.hostname}`;
    document.cookie = `${Cookies.customerUri}=${response.data.user.uri};domain=${window.location.hostname}`;
}

export const GetCookies = () => {
    const token = document
        .cookie
        .split(';')
        .find(x => x.trim().startsWith(Cookies.token))
        ?.split('=')[1];

    const refreshToken = document
        .cookie
        .split(';')
        .find(x => x.trim().startsWith(Cookies.refreshToken))
        ?.split('=')[1];
    
        const customerUri = document
        .cookie
        .split(';')
        .find(x => x.trim().startsWith(Cookies.customerUri))
        ?.split('=')[1];
    
    return {
        token,
        refreshToken,
        customerUri,
    };
};

export const DeleteAllCookies = () => {
    document.cookie
        .split(";")
        .map((cookie) => {
            const cookiesName: string = cookie.split("=")[0].trim();
            Object.values(Cookies).some(x => x == cookiesName) && (document.cookie = `${cookiesName}=${cookie}; expires=${new Date(1970, 1, 1)}; path=/;`);
        });
}

