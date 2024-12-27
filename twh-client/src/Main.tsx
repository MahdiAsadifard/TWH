import React, { useState } from 'react';

import LoginContainer from './Login/LoginContainer';
import {  useNavigate } from 'react-router';
import { paths } from './Routs/Router';

import "./Styles/Commons.css";

// TODO: handle auto login - re generate token
const Main = () => {
  const navigate = useNavigate();

  const loginCallback = (val: any) => {
    navigate(paths.signedin);
  }
  return (
    <div className='root'>
      <LoginContainer callback={(e: any)=>loginCallback(e)} />
    </div>
  );
}

export default Main;
