const applicationServerPublicKey = 'BB1VbsmgXGEqWHarrfij5e2FAihZzs-t5GyC9LwxZyYESZmFC93lYzMbKWhB23nojBr50_BOFBXu-eXniCiOWzo';
let isSubscribed = false;
let swRegistration = null;
var activeSubscription = null;

function urlB64ToUint8Array(base64String) {
    const padding = '='.repeat((4 - base64String.length % 4) % 4);
    const base64 = (base64String + padding)
      .replace(/\-/g, '+')
      .replace(/_/g, '/');

    const rawData = window.atob(base64);
    const outputArray = new Uint8Array(rawData.length);

    for (let i = 0; i < rawData.length; ++i) {
        outputArray[i] = rawData.charCodeAt(i);
    }
    return outputArray;
}

if ('serviceWorker' in navigator && 'PushManager' in window) {
    console.log('Service Worker and Push supported');

    navigator.serviceWorker.register('./Scripts/optikoud.serviceworker.js').then(
        function (swReg) {
            console.log('Service Worker registered', swReg);
            swRegistration = swReg;
            initialiseUI();
        }).catch(
        function (error) {
            console.error('Service Worker Error', error);
        });
}
else {
    console.warn('Push messaging is not supported');
    $('.notifications-activate').addClass('disabled');
    $('.notifications-activate label').html('NOTIFICACIONES NO SOPORTADAS');
}

function subscribePush() {
    if (isSubscribed) {
        unsubscribeUser();
    } else {
        subscribeUser();
    }
}

function initialiseUI() {
    // Set the initial subscription value
    swRegistration.pushManager.getSubscription().then(
        function (subscription) {
            isSubscribed = !(subscription === null);

            if (isSubscribed) {
                activeSubscription = subscription;
                console.log('User IS subscribed.');
            } else {
                console.log('User is NOT subscribed.');
            }
            subscribeUser();

            updateBtn();
        });
}

function updateBtn() {
    if (Notification.permission === 'denied') {
        $('.notifications-activate').addClass('disabled');
        $('.notifications-activate label').html('NOTIFICACIONES BLOQUEADAS');
        updateSubscriptionOnServer(null);
        return;
    }

    if (isSubscribed) {
        $('.notifications-activate label').html('DESACTIVAR NOTIFICACIONES');
    } else {
        $('.notifications-activate label').html('ACTIVAR NOTIFICACIONES');
    }

    $('.notifications-activate').removeClass('disabled');
}

function subscribeUser() {
    const applicationServerKey = urlB64ToUint8Array(applicationServerPublicKey);
    swRegistration.pushManager.subscribe({
        userVisibleOnly: true,
        applicationServerKey: applicationServerKey
    }).then(
        function (subscription) {
            console.log('User is subscribed:', subscription);
            activeSubscription = subscription;
            updateSubscriptionOnServer(subscription);

            isSubscribed = true;

            updateBtn();
        }).catch(
            function (err) {
                console.log('Failed to subscribe the user: ', err);
                updateBtn();
            });
}

function unsubscribeUser() {
    swRegistration.pushManager.getSubscription().then(
        function (subscription) {
            if (subscription) {
                activeSubscription = subscription;
                return subscription.unsubscribe();
            }
        }).catch(
        function (error) {
            console.log('Error unsubscribing', error);
        }).then(
        function () {
            updateSubscriptionOnServer(null);

            console.log('User is unsubscribed.');
            isSubscribed = false;

            updateBtn();
        });
}

function updateSubscriptionOnServer(subscription) {

    if (subscription) {
        $.post('/Notificaciones/Suscribir', { Subscription: JSON.stringify(subscription) },
            function (data) {
                if (data.OK) {
                }
                else {

                }
            });
    } else {
        $.post('/Notificaciones/Desuscribir', { Subscription: JSON.stringify(activeSubscription) },
            function (data) {
                if (data.OK) {
                }
                else {

                }
            });
    }
}