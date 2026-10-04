export default async function globalTeardown() {
  await globalThis.__MONGOD__?.stop();
}
