self.addEventListener('push', function (event) {
    console.log('[Service Worker] Push Received.');
    var jData = JSON.parse(event.data.text())

    const title = jData.title;
    const options = {
        body: jData.body,
        icon: '/valoracion/Images/iso.png',
        badge: '/valoracion/Images/iso.png',
        data: {
            url: jData.url
        }
    };

    const notificationPromise = self.registration.showNotification(title, options);
    event.waitUntil(notificationPromise);
});

self.addEventListener('notificationclick', function (event) {
    console.log('[Service Worker] Notification click Received.');

    event.notification.close();
    console.log('Url:' + event.notification.data.url);

    event.waitUntil(
      clients.openWindow(event.notification.data.url)
    );
});