import { FluentProvider, webLightTheme, webDarkTheme } from "@fluentui/react-components"
import { RouterProvider }  from "react-router"
import { router } from './Routs/Router';

function App() {


  return (
    <FluentProvider theme={webLightTheme}>
      <RouterProvider router={router} />
    </FluentProvider>
  );
}

export default App;
