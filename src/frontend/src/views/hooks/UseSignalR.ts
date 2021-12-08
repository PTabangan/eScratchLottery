import { useEffect, useState } from 'react'
import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr'

interface UseSignalRProp{
    url: string;
    methodName: string;
    onReceived(data : any): void;
}

export const useSignalR = (prop: UseSignalRProp ) => {
    const [connection, setConnection] = useState<HubConnection | null>(null); 

    useEffect(() => {
        (() => {
            const conn = new HubConnectionBuilder()
                .withUrl(prop.url)
                .withAutomaticReconnect()
                .build();
            setConnection(conn);
        })();

    }, [prop.url, prop.methodName])

    useEffect(()=> {
        (async () => {
            if (connection) {
                try {
                    await connection.start();
                    console.log('SignalR client is now connected')
                    
                    connection.on( prop.methodName, message => {
                        prop.onReceived(message);
                    });
                } catch (error) {

                }
            }
        })()
    }, [connection, prop.url, prop.methodName, prop.onReceived])

    return [connection];
};