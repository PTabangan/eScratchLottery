import React, { FC, useState, useEffect } from 'react';
import Container from '@mui/material/Container';
import CssBaseline from '@mui/material/CssBaseline';
import { createTheme, ThemeProvider } from '@mui/material/styles';

import Register from './views/pages/register';
import ScratchLottery from './views/pages/scratchLottery'

const theme = createTheme();

const App: FC = () => {
  const [playerName, setPlayerName] = useState<string | null>(null);

  const _onPlayerEnter = (name: string):void =>{
      const trimValue = name.trim();
      if (trimValue){
        window.sessionStorage.setItem("playerName", name);
        setPlayerName(name);
      }
  }

  const _onPlayerLeave = ():void =>{
      window.sessionStorage.removeItem("playerName");
      setPlayerName(null);
  }

  useEffect(() => {
      (async() => {
        setPlayerName(window.sessionStorage.getItem("playerName") || '')
      })()
  }, [])


  return (
    <ThemeProvider theme={theme}>
      <Container component="main" maxWidth="lg">
        <CssBaseline />
        { playerName 
          ? <ScratchLottery playerName={playerName} onPlayerLeave={_onPlayerLeave} />
          : <Register
              onPlayerEnter={_onPlayerEnter}
            />
        }
      </Container>
    </ThemeProvider>
  );
}

export default App;
