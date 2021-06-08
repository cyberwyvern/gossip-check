const path = require('path');

const resolvePath = p => path.resolve(__dirname, p)

module.exports = {
  webpack: {
    alias: {
      '@shared': resolvePath('./src/shared'),
      '@pages': resolvePath('./src/pages'),
      '@api': resolvePath('./src/api-client'),
      '@stores': resolvePath('./src/stores'),
      '@config': resolvePath('./src/config.json')
    }
  },
}