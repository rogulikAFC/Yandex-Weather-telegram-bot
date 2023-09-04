import { useEffect, useRef, useState } from "react";
import Modal from "../Modal/Modal";
import Button from "../Button/Button";
import Field from "../Form/Field/Field";
import PlaceChangeForm from "../PlaceChangeForm/PlaceChangeForm";
import "./ChangePlaceModal.css";

function ChangePlaceModal({
  placeId,
  userId,
  show,
  blockName,
  unshowChangeModal,
}) {
  const [place, setPlace] = useState(null);

  useEffect(() => {
    async function GetPlace() {
      if (show == false || userId == null) {

        return;
      }

      if (placeId == null) {

        setPlace(null);
        unshowChangeModal();

        return;
      }

      const url = `https://localhost:7137/api/Places/${placeId}?user_id=${userId}`;

      var response = await fetch(url);

      if (!response.ok) {
        console.log("some error occured");

        return;
      }

      var result = await response.json();

      setPlace(result);
    }

    GetPlace();
  }, [show, placeId]);

  async function deletePlace() {
    const url = `https://localhost:7137/api/Places/${placeId}?user_id=${userId}`;

    var response = await fetch(url, {
      method: "DELETE",
    });

    if (!response.ok) {
      console.log("some error occured");

      return;
    }

    window.location.reload();
  }

  return (
    <Modal show={show} blockName={blockName}>
      <h2 className="modal__heading">Changing "{place?.name}" place</h2>

      <PlaceChangeForm
        place={place}
        blockName="modal"
        placeId={placeId}
        userId={userId}
      />

      <div className="modal-actions modal__modal-actions modal__modal-actions_place-change-form">
        <Button
          heightInPx={46.5}
          widthInPx={150}
          color="red"
          block="modal-actions"
          onClick={deletePlace}
        >
          Delete
        </Button>

        <Button
          heightInPx={46.5}
          widthInPx={150}
          color="white"
          block="modal-actions"
          onClick={unshowChangeModal}
        >
          Close
        </Button>

        <Button
          heightInPx={46.5}
          widthInPx={150}
          color="blue"
          block="modal-actions"
          submit={true}
          other={{
            form: "place-change-form",
          }}
        >
          Save
        </Button>
      </div>
    </Modal>
  );
}

export default ChangePlaceModal;
