import { ILoginReponse } from "../Login/Types";


export enum Cookies {
    token = 'twh_token',
    refreshToken = 'twh_refresh_token',
    customerUri = 'twh_customer_uri',
    rememberMe = 'twh_remember_me',
};

export const SetCookies = (response: ILoginReponse) => {
    const tokenExpiryDate = new Date(response.data.token.accessTokenExpityUTC);
    const refreshTokenExpiryDate = new Date(response.data.token.refreshTokenExpityUTC);

    document.cookie = `${Cookies.token}=${response.data.token?.accessToken};expires=${tokenExpiryDate.toUTCString()};domain=${window.location.hostname}`;
    document.cookie = `${Cookies.refreshToken}=${response.data.token?.refreshToken};expires=${refreshTokenExpiryDate.toUTCString()};domain=${window.location.hostname}`;
    document.cookie = `${Cookies.customerUri}=${response.data.user.uri};domain=${window.location.hostname}`;
    document.cookie = `${Cookies.rememberMe}=${response.rememberMe ? 'true' : 'false'};domain=${window.location.hostname}`;
}

export const GetCookies = () => {
    const map = new Map();
    document.cookie.split(';').map(browserCookie => {

        const [key, value] = browserCookie.split('=').map(c => c.trim());
        const browserIncludesCookie = Object.values(Cookies).some(x => x == key);

        if(browserIncludesCookie) {
            for (const [cKey, cValue] of Object.entries(Cookies)) {
                if (cValue === key) {
                    map.set(cKey, value);
                    break;
                }
            }
        }
    });

    const result = Object.fromEntries(map)      
    return { ...result };
};

export const DeleteAllCookies = () => {
    document.cookie
        .split(";")
        .map((cookie) => {
            const cookiesName: string = cookie.split("=")[0].trim();
            Object.values(Cookies).some(x => x == cookiesName) && (document.cookie = `${cookiesName}=${cookie}; expires=${new Date(1970, 1, 1)}; path=/;`);
        });
}

