// Ionic Starter App

// angular.module is a global place for creating, registering and retrieving Angular modules
// 'starter' is the name of this angular module example (also set in a <body> attribute in index.html)
// the 2nd parameter is an array of 'requires'
angular.module('GolGuruApp', ['ionic', 'GolGuruApp.controllers', 'GolGuruApp.directives'])


.run(function($ionicPlatform) {
  $ionicPlatform.ready(function() {
    if(window.StatusBar) {
      StatusBar.styleDefault();
    }
  });
})

/**
 * The Projects factory handles saving and loading projects
 * from local storage, and also lets us save and load the
 * last active project index. 
 * IMPORTNAT: FOR QUESTIOSN BEFORE MATCH START, IN EstadisticaCtrl WE CREATE THE ELEMENT IN PROJECT ARRAY FOR ALL THOSE QUESTIONS WITH EMTY ANSWER. THE IS UPDATED IN PreguntaEstadisticaOpenCtrl
 * IMPORTANT: THE ID OF THE QUESTIONS - 1 IS EQUAL TO THE POSITION IN PROJECTS ARRAY
 * IMPORTANT: IN LOCAL STORAGE ARE SAVED THE QUESTION ID + THE ANSWER STRING. IN SERVER SEND QUESTIONS FOLLOWING ORDER OF ID FOR FINAL VALIDATION OF THE ANSWERS*/
 
.factory('Projects', function() {
  return {
    all: function() {
      var projectString = window.localStorage['projects'];
      if(projectString) {
        return angular.fromJson(projectString);
      }
      return [];
    },
    save: function(projects) {
      window.localStorage['projects'] = angular.toJson(projects);
    },
    newProject: function(projectTitle, respuesta) {
      // Add a new project
      return {
        id: projectTitle,
        respuesta: respuesta,
      };
    }
  }
})

.config(['$urlRouterProvider', '$stateProvider', function($urlRouterProvider, $stateProvider) {
  
  $urlRouterProvider.otherwise('/');
  
  $stateProvider
  	  //Log In screen
	  .state('index', {
	    url: '/',
	    templateUrl: 'templates/log_in.html',
	    controller: 'LogInCtrl'
	  })

	  //When no question is display. Before match, show button to go to estadistica.html, when math start hide button.
	  .state('antes_partido', {
	    url: '/antes_partido',
	    templateUrl: 'templates/antes_partido.html',
	    controller: 'AntesPartidoCtrl'
	  })

	  //Show questions before match
	  .state('estadistica', {
	    url: '/estadistica',
	    templateUrl: 'templates/estadistica.html',
	    controller: 'EstadisticaCtrl',
	    resolve: {
	    	tiempo_jugadores: ['$http', function($http) {
	    		return $http.get('js/JSON/tiempo_jugadores.json').then(function(response) {
    				return response.data;
	    		})
	    	}],
	    	preguntas_estadisticas: ['$http', function($http) {
	    		return $http.get('js/JSON/preguntas_estadisticas.json').then(function(response) {
    				return response.data;
	    		})
	    	}]
	    }
	  })

  	  //Question from estadistics
  	  .state('pregunta_estadistica_open', {
	    url: '/pregunta_estadistica_open',
	    templateUrl: 'templates/pregunta_estadistica_open.html',
	    controller: 'PreguntaEstadisticaOpenCtrl'
	  })

  	  //Posiciones -> Show at the end of the game
	  .state('posiciones', {
	    url: '/posiciones',
	    templateUrl: 'templates/posiciones.html',
	    controller: 'PosicionesCtrl',
	    resolve: {
	    	posiciones: ['$http', function($http) {
	    		return $http.get('js/JSON/posiciones.json').then(function(response) {
    				return response.data;
	    		})
	    	}]
	    }
	  })
}]);


