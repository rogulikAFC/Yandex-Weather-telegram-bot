import Button from "../Button/Button";
import "./PlaceCard.css";

function PlaceCard({
  place: { id, lat, lon, name, mapImageBase64, isMain },
  blockName,
  onChangeButtonClick
}) {
  mapImageBase64 = "data:image/png;base64," + mapImageBase64;

  return (
    <div
      className={`place-card ${blockName}__place-card${
        isMain ? " place-card__main" : ""
      }`}
    >
      <img
        src={mapImageBase64}
        alt="map image"
        className="place-card__map-image"
      />
      <div className="place-card__data-container">
        <h3 className="place-card__name">{name}</h3>
        <table className="location-table place-card__location-table">
          <tbody className="location-table__body">
            <tr className="location-table__row">
              <th className="location-table__heading">Latitude:</th>
              <td className="location-table__value">{lat}</td>
            </tr>
            <tr className="location-table__row">
              <th className="location-table__heading">Longitude:</th>
              <td className="location-table__value">{lon}</td>
            </tr>
          </tbody>
        </table>

        <Button
          color={isMain ? "white" : "blue"}
          submit={false}
          heightInPx={30}
          widthInPx={210}
          block="place-card"
          onClick={onChangeButtonClick}
        >
          Change
        </Button>
      </div>
    </div>
  );
}

export default PlaceCard;
