var Octokit = require('octokit-rest-for-node-v0.12');

var createOctokit = function (appName, host) {
    var octokitConfiguration = {
        timeout: 0,
        requestMedia: 'application/vnd.github.v3+json',
        headers: {
            'user-agent': appName
        }
    };

    if (host) {
        octokitConfiguration.host = host;
        octokitConfiguration.pathPrefix = 'api/v3';
    }

    return Octokit(octokitConfiguration);
};

module.exports = { createOctokit: createOctokit };