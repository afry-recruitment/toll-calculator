import NodeCache from 'node-cache';

export default class appCache {
  constructor() {
    this.cache = new NodeCache();
  }
  get(key) {
    return this.cache.get(key);
  }
  set(key, value) {
    return this.cache.set(key, value);
  }
  del(key) {
    return this.cache.del(key);
  }
  has(key) {
    return this.cache.has(key);
  }
}
