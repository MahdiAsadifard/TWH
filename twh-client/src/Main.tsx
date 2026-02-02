import React from 'react';

import LoginContainer from './Login/LoginContainer';
import {  useNavigate } from 'react-router';
import { paths } from './Routs/Router';

import UseFetch from './Loggedin/Common/UseFetch';
import { useUser } from './Loggedin/Common/UserProvider';
import * as Utils from "./Global/Utils";

import "./Styles/Commons.css";
import { IUserReponseDto } from './Login/Types';

const Main = () => {
  const navigate = useNavigate();

  
  React.useEffect(() => {
    checkCookies();
  },[]);

  const { ApiRequest, fetchResponse, error, loading } = UseFetch();
  const { setUser } = useUser();

  const getUserAndSetLocalStorage = async(uri: string) => {
    await ApiRequest({
        url: `${uri}/user/${uri}`,
        method: "GET",
        withToken: true
    });
    console.log("==fetc uti: =",fetchResponse);
    if(!error && fetchResponse.success){
      setUser(fetchResponse.response.data as IUserReponseDto);
    }
  };

  const checkCookies = async () => {
    const { token, refreshToken, customerUri, rememberMe } = Utils.GetCookies();
    const remember = JSON.parse(rememberMe?.toLowerCase() ?? false);
    if(remember && token && refreshToken && customerUri){
     await getUserAndSetLocalStorage(customerUri);
     navigate(paths.signedin);
    }
  };

  const loginCallback = (val: boolean) => {
    if(val) {
      checkCookies();
      navigate(paths.signedin);
    }
  };

  return (
    <div className='root'>
      <LoginContainer callback={(e: any)=>loginCallback(e)} />
    </div>
  );
}

export default Main;
