
const isDevelopment = process.env.NODE_ENV.includes('development');

export const WebservicePort =  isDevelopment ? '5005' : '5006';
export const WebserviceHost = `http://localhost:`;

export const WebserviceUrl = `${WebserviceHost}${WebservicePort}/api/`;