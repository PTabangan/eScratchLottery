import React, { FC } from 'react';
import { 
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    Typography,
    Slide
} from '@mui/material';
import { TransitionProps } from '@mui/material/transitions';

interface PriceDialogSlide{
    wonPrice?: string | null;
    open: boolean;
    onClose(): void;
}


const Transition = React.forwardRef(function Transition(
    props: TransitionProps & {
            children: React.ReactElement<any, any>;
    },
    ref: React.Ref<unknown>,
) {
    return <Slide direction="up" ref={ref} {...props} />;
});


const PriceDialogSlide: FC<PriceDialogSlide> = (prop:PriceDialogSlide) => ((
      <Dialog
        open={prop.open}
        TransitionComponent={Transition}
        keepMounted
        onClose={prop.onClose}
        aria-describedby="alert-dialog-slide-description"
      >
        <DialogContent sx={{ textAlign: "center" }}>
            { prop.wonPrice 
                ? (
                  <>
                    <Typography 
                        variant="h3" 
                        gutterBottom 
                        component="div" 
                        sx={{ fontFamily: "Roboto", fontWeight: "bold" }} 
                    >
                        YOU WON!
                    </Typography>
                    <Typography 
                        variant="h1" 
                        gutterBottom 
                        component="div" 
                        sx={{ fontFamily: "Roboto", fontWeight: "bold" }} 
                    >
                        {prop.wonPrice}
                    </Typography>
                 </>
                )
                : (
                    <>
                      <Typography 
                        variant="h3" 
                        gutterBottom 
                        component="div" 
                        sx={{ fontFamily: "Roboto", fontWeight: "bold" }} 
                     >
                          Sorry!
                     </Typography>
                     <Typography 
                        variant="h3" 
                        gutterBottom 
                        component="div" 
                        sx={{ fontFamily: "Roboto", fontWeight: "bold" }} 
                     >
                          Better luck next time.
                     </Typography>
                    </>
                )
            }
        </DialogContent>
        <DialogActions>
          <Button onClick={prop.onClose}>Close</Button>
        </DialogActions>
      </Dialog>
  ));

export default PriceDialogSlide; 