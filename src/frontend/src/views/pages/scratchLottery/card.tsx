import React, { FC } from 'react';
import {
    Grid,
    Paper,
    Typography 
}from '@mui/material';

interface CardProp{
    id: number;
    name: string;
    playerName?: string;
    wonPrice?: string;
    readOnly: boolean;
    onCardReveal(id: number): void
}

interface PaperAttribute {
    backgroundColor: string;
    onSelectCard(): void;
}

const Card: FC<CardProp> = (prop: CardProp) => {

    const paperAttribute: PaperAttribute = {
        backgroundColor:(prop.readOnly || prop.playerName) 
            ? '#d3d3d3'
            : '#76d8E3',
        onSelectCard: (prop.readOnly || prop.playerName)
            ? ()=>{} 
            : () => { prop.onCardReveal(prop.id) }
    }
    
    return(
    <Grid key={prop.id} item justifyContent="center"  >
        <Paper 
            sx={{ height: 110, width: 110, margin: "auto", backgroundColor: paperAttribute.backgroundColor, textAlign: 'center',}} 
            elevation={4} 
            onClick={()=>{paperAttribute.onSelectCard()}}
        >
            <Typography variant="button" display="block">
                {String(prop.id).padStart(4, '0')}
            </Typography>
            { prop.playerName 
                ?(
                  <>  
                    <Typography  variant="h4" gutterBottom component="div" sx={{fontFamily:'Roboto', fontWeight: 'bold'}}>
                        { prop.wonPrice ?? '0.00' }
                    </Typography>
                    <Typography  variant="caption" display="block" gutterBottom>
                        {prop.playerName}
                    </Typography>
                  </>  
                 )
                :(
                    <Typography gutterBottom variant="h3" component="span" sx={{fontFamily:'Roboto', fontWeight: 'bold'}}>
                        $$
                    </Typography>
                 ) 
            }
        </Paper>    
    </Grid>
)
};

export default Card;