import React, { useState } from 'react';

import LoginContainer from './Login/LoginContainer';
import {  useNavigate } from 'react-router';
import { paths } from './Routs/Router';

// TODO: handle auto login
const Main = () => {
  const navigate = useNavigate();

  const loginCallback = (val: any) => {
    navigate(paths.signedin);
  }
  return (
    <>
      <LoginContainer callback={(e: any)=>loginCallback(e)} />
    </>
  );
}

export default Main;
