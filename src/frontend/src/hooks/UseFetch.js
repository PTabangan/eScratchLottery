import { useEffect, useState } from 'react'

export const useFetch = (url) => {
    const [isLoading, setIsLoading] = useState(true);
    const [data, setData] = useState(null);

    useEffect(() => {
        let cancelled = false;
        (async () => {
            setIsLoading(true);

            const response = await fetch(url, { method: 'GET', mode: 'no-cors' });
            if (response.ok){
                const data = await response.json();
                if (!cancelled){
                    setData(data);
                    setIsLoading(false);
                }
            }

            if (!response.ok){
                if (!cancelled){
                    console.log('Error fetching data')
                }
            }
        })();
        
        return () => {
            cancelled = true;
        }
    }, [url]);

    return [isLoading, data];
};

export const post = async(url, data) => {
    const response = await fetch(url, {
        method: "POST",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    });

    return response;
}

