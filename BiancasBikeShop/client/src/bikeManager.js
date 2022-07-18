const apiUrl = '/api/bike';

export const getBikes = () => {
    //add implementation here... 
    return fetch(apiUrl).then((response)=>response.json())
}

export const getBikeById = (id) => {
    //add implementation here... 
    return fetch(`${apiUrl}/${id}`).then((response)=>response.json())
}

export const getBikesInShopCount = () => {
    //add implementation here... 
}