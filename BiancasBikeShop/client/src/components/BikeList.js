import { useState, useEffect } from "react"
import { getBikes } from "../bikeManager"
import BikeCard from "./BikeCard"

export default function BikeList({setDetailsBikeId}) {
    const [bikes, setBikes] = useState([])

    const getAllBikes = () => {
        //implement functionality here...
        getBikes().then((fetchedBikes)=>{
            setBikes(fetchedBikes)
        })
    }

    useEffect(() => {
        getAllBikes()
    }, [])
    return (
        <>
            <h2>Bikes</h2>
            {/* Use BikeCard component here to list bikes...*/}
            {(bikes.length === 0)?
            
                <>
                    <p>Loading...</p>
                </>
                :
                <>
                {bikes.map((bike)=>(
                    <BikeCard bike={bike} key={bike.id} setDetailsBikeId={setDetailsBikeId}/>
                ))}
                </>
            }
        </>
    )
}