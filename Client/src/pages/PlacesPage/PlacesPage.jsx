import { useEffect, useRef, useState } from "react";
import "./PlacesPage.css";
import PlaceCard from "../../PlaceCard/PlaceCard";
import { useParams } from "react-router-dom";
import ChangePlaceModal from "../../ChangePlaceModal/ChangePlaceModal";

function PlacesPage() {
  const { userId } = useParams();
  const [places, setPlaces] = useState([]);
  const [selectedPlaceId, setSelectedPlaceId] = useState(null);
  const [showChangeModal, setShowChangeModal] = useState(false);

  const isInitialMount = useRef(true);

  useEffect(() => {
    if (isInitialMount.current) {
      isInitialMount.current = false;
      
      return;
    }

    console.log(selectedPlaceId);
    setShowChangeModal(true);
  }, [selectedPlaceId]);

  useEffect(() => {
    async function GetPlaces() {
      const url = `https://localhost:7137/api/Places/by_user/${userId}?user_id=${userId}`;

      var response = await fetch(url);
      var result = await response.json();
      result.sort((a, b) => a.isMain < b.isMain);

      setPlaces(result);
    }

    GetPlaces();
  }, []);

  function unshowChangeModal() {
    setShowChangeModal(false);
    setSelectedPlaceId(null);
  }

  return (
    <div className="places-page">
      {places.map((place) => (
        <PlaceCard
          key={place.id}
          place={place}
          blockName="places-page"
          onChangeButtonClick={() => setSelectedPlaceId(place.id)}
        />
      ))}

      <ChangePlaceModal
        userId={userId}
        placeId={selectedPlaceId}
        show={showChangeModal}
        unshowChangeModal={unshowChangeModal}
        blockName="places-page"
      />
    </div>
  );
}

export default PlacesPage;
