import React, { FC, useState, useEffect } from 'react';
import { 
    Toolbar,
    Typography,
    Button,
    Grid,
    CircularProgress,
    Snackbar,
    Alert 
} from '@mui/material';

import Card from './card';
import PriceDialogSlide from './priceDialog';
import { useFetch, postJson } from '../../hooks/UseFetch';
import { useSignalR } from '../../hooks/UseSignalR';

interface ScratchLotteryProp{
    onPlayerLeave(): void;
    playerName: string;
}

interface CardDto {
    id: number;
    name: string;
    playerName: string;
    wonPrice: string;
}

const Index : FC<ScratchLotteryProp> = (prop: ScratchLotteryProp) => {
    const [isLoading, data] = useFetch([],'/api/cards');
    const [cards, setCards] = useState<CardDto[]>([]);
    const [revealedCard, setRevealedCard] = useState<CardDto | null>(null);
    const [connection] = useSignalR({
        url: 'http://localhost:2020/hubs/player', 
        methodName:'RevealedCard',
        onReceived: m => setRevealedCard(m)
    });
    // TO DO rename the variable and complete the build and run process//
    const [readOnly, setReadOnly]   = useState<boolean>(false);
    const [showPrice, setShowPrice] = useState<boolean>(false);
    const [wonPrice, setWonPrice] = useState<string | null>(null);
    const [error, setError] = useState<string>('');
    
    useEffect(() => {
        if (!data){
            return;
        }
        if (!cards.length){
            const items = data.map((p: CardDto) => ({
                id: p.id,
                name: p.name,
                playerName: p.playerName,
                wonPrice: p.wonPrice
            }));
            setCards(items);
            const playerAlreadyPlayed = items.filter((p:any) => p.playerName?.toUpperCase() === prop.playerName.toUpperCase()).length > 0;
            setReadOnly(playerAlreadyPlayed);
        }

        if (revealedCard){
            const items = cards.map(item => (item.id === revealedCard.id 
                ?{
                  ...item, 
                  playerName: revealedCard.playerName, 
                  wonPrice: revealedCard.wonPrice  
                } : item));
            setCards(items);
        }
    },[data, revealedCard])

    const _cardReveal = async(id: number) => {
        const response =  await postJson('/api/cards', {
            cardId: id,
            playerName: prop.playerName
        });
        
        if (response.ok){
            setWonPrice(response.data.value);
            setShowPrice(true);
        }

        if (!response.ok){
            setError(response.data.message);
        }
    }

    const _onPriceDialogClose = ():void =>{
        setReadOnly(true);
        setShowPrice(false);
    }

    if (isLoading || connection === null){
        return(<CircularProgress style={{
            display: 'block',
            margin: '30vh auto 0 auto'
        }}/>)
    }

    return(
        <>
            <Toolbar sx={{ borderBottom: 1, borderColor: 'divider' }}>
                <Typography
                    component="span"
                    variant="h6"
                    color="inherit"
                    align="left"
                    noWrap
                    sx={{ flex: 1 }}
                >
                    {`Player: ${prop.playerName}`}   
                </Typography>
                <Button variant="outlined" size="small" onClick={prop.onPlayerLeave}>
                    Leave
                </Button>
            </Toolbar>
            <Grid sx={{ flexGrow: 1 }} container spacing={2}>
                <Grid item xs={12}>
                    <Grid container justifyContent="center" spacing={0.5}>
                        {cards.map(card => 
                            <Card 
                                id={card.id} 
                                name={card.name}
                                playerName={card.playerName} 
                                wonPrice={card.wonPrice} 
                                onCardReveal={_cardReveal}
                                readOnly={readOnly}
                            />)}
                    </Grid>
                </Grid>
            </Grid>
            <PriceDialogSlide open={showPrice} wonPrice={wonPrice} onClose={_onPriceDialogClose}  />
            <Snackbar 
                open={error !== ''} 
                autoHideDuration={6000}
                anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
                onClose={()=>{ setError('') }}
            >
                <Alert severity="warning">{error}!</Alert>
            </Snackbar>
            <Snackbar 
                open={readOnly} 
                autoHideDuration={6000}
                anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
            >
                <Alert severity="info">Thank you for participating in the lottery. To participate again, leave and use a different player name.</Alert>
            </Snackbar>
        </>
    );
}

export default Index;
