import React, { useState } from 'react';
import { FluentProvider, webLightTheme, webDarkTheme } from "@fluentui/react-components"
import './App.css';

import LoginContainer from './Login/LoginContainer';
import HomeContainer from './Loggedin/Home/HomeContainer';

function App() {
  const [loggin, setLoggin] = useState(false);

  const loginCallback = (val: any) => {
    setLoggin(true);
  }

  return (
    <FluentProvider theme={webLightTheme}>
      {loggin ? (
          <HomeContainer />
        ): (
          <LoginContainer callback={(e: any)=>loginCallback(e)} />
      )}
    </FluentProvider>
  );
}

export default App;
