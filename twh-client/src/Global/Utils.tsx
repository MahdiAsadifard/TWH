
export enum Cookies {
    token = 'twh_token',
    newRefreshToken = 'twh_refresh_token'
};

export const GetTokens = () => {
    const token = document
        .cookie
        .split(';')
        .find(x => x.trim().startsWith(Cookies.token))
        ?.split('=')[1];

    const refreshToken = document
        .cookie
        .split(';')
        .find(x => x.trim().startsWith(Cookies.newRefreshToken))
        ?.split('=')[1];;
    
    return {
        token,
        refreshToken
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

