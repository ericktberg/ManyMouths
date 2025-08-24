## Update

So I've started making the transition to React native, something I know very little about. Hopefully I can learn more.

The current plan is to design using react, which can be launched through expo as a local web-app. When it comes time to deploy to mobile, this can be built to a binary using EAS. EAS has a free tier which should be good enough.

The figma that is going to be the source of the design will be https://www.figma.com/make/6x8tgIjuL9Vlt0C2gyIxkv/Food-Cost-Tracking-App?node-id=0-1&p=f&t=HgpVk0pJF5bueMYT-0

---

## Local Deploy

To start the application
1. start the MySQL service
2. Open the PriceCheck.DB solution in backend and launch that
3. Launch the frontend using `expo start` or `npm run start`