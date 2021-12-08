import {useState, useEffect} from 'react';

interface PostResult{
    ok: boolean;
    data: any;
}

export const postJson =async(url : string, data: any):Promise<PostResult> => {
    const result = { ok: false, data: null };
    const response = await fetch(url, {
        method: "POST",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    });

    result.ok = response.ok;
    result.data = await response.json();

    return result;
}

export const useFetch = (defaultData: any, url : string): [boolean, any] =>  {
    const [isLoading, setIsLoading] = useState(true);
    const [data, setData] = useState(defaultData);

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
}
