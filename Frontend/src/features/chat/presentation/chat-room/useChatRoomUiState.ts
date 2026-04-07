import { useEffect, useRef, useState } from 'react';

export function useChatRoomUiState() {
  const [showPaymentOffer, setShowPaymentOffer] = useState(false);
  const [showDisputeModal, setShowDisputeModal] = useState(false);
  const [rejectReason, setRejectReason] = useState('');
  const [disputeReason, setDisputeReason] = useState('');
  const [showActionMenu, setShowActionMenu] = useState(false);
  const actionMenuRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    const onMouseDown = (event: MouseEvent) => {
      const target = event.target as Node;
      if (actionMenuRef.current && !actionMenuRef.current.contains(target)) {
        setShowActionMenu(false);
      }
    };

    document.addEventListener('mousedown', onMouseDown);
    return () => document.removeEventListener('mousedown', onMouseDown);
  }, []);

  return {
    actionMenuRef,
    disputeReason,
    rejectReason,
    setDisputeReason,
    setRejectReason,
    setShowActionMenu,
    setShowDisputeModal,
    setShowPaymentOffer,
    showActionMenu,
    showDisputeModal,
    showPaymentOffer,
  };
}
