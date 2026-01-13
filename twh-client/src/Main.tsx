import React, { useState } from 'react';

import LoginContainer from './Login/LoginContainer';
import {  useNavigate } from 'react-router';
import { paths } from './Routs/Router';

import * as Utils from "./Global/Utils";

import "./Styles/Commons.css";

const Main = () => {
  const navigate = useNavigate();

  
    React.useEffect(() => {
        const { token, refreshToken, customerUri, rememberMe } = Utils.GetCookies();
        
        const remember = JSON.parse(rememberMe?.toLowerCase() ?? false);
        if(remember && token && refreshToken && customerUri){
            loginCallback(true);
        }
    },[]);

  const loginCallback = (val: boolean) => {
    navigate(paths.signedin);
  }
  return (
    <div className='root'>
      <LoginContainer callback={(e: any)=>loginCallback(e)} />
    </div>
  );
}

export default Main;
