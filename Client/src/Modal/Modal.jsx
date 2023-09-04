import { forwardRef, useImperativeHandle } from "react";
import "./Modal.css";

function Modal({ blockName, children, show }) {
  // useImperativeHandle(
  //   ref,
  //   () => {
  //     return {

  //     }
  //   }
  // )

  return (
    <div
      className={`modal-wrapper ${blockName}__modal-wrapper ${
        show ? "" : "modal-wrapper_hidden"
      }`}
    >
      <div className="modal modal-wrapper__modal">{children}</div>
    </div>
  );
}

export default Modal;
// export const Modal = forwardRef(ModalInner);
