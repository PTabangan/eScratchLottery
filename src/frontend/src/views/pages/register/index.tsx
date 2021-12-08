import React, { FC }from 'react';
import {
    Box,
    Button,
    TextField,
    Typography
} from '@mui/material';

interface RegisterProp
{
    onPlayerEnter(playerName : string): void;
};

const Index: FC<RegisterProp> =  (prop: RegisterProp) => {

 const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const data = new FormData(event.currentTarget);
    const playerName = data.get('playerName')?.toString() ?? '';
    prop.onPlayerEnter(playerName)
  };

  return (
        <Box
          sx={{ marginTop: 8, display: 'flex', flexDirection: 'column', alignItems: 'center' }}
        >
          <Typography component="h1" variant="h5">
            Join the Scratch Lottery
          </Typography>
          <Box component="form" noValidate onSubmit={handleSubmit} sx={{ mt: 3 }}>
            <TextField
                autoComplete="given-name"
                name="playerName"
                required
                fullWidth
                id="playerName"
                label="Player's Name"
                autoFocus
            />
            <Button
              type="submit"
              fullWidth
              variant="contained"
              sx={{ mt: 3, mb: 2 }}
            >
              Enter
            </Button>
          </Box>
        </Box>
  );
}

export default Index;
