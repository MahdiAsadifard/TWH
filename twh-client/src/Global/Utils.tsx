
export const GetTokenFromCookies = () => {
    return document
        .cookie
        .split(';')
        .find(x => x.trim().startsWith('twh_token'))
        ?.split('=')[1];
};