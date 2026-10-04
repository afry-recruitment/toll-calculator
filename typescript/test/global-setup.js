import { MongoMemoryServer } from 'mongodb-memory-server';

export default async function globalSetup() {
  const server = await MongoMemoryServer.create();
  process.env.MONGODB_URI = server.getUri();
  globalThis.__MONGOD__ = server;
}
