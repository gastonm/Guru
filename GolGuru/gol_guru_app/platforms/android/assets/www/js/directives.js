angular.module('GolGuruApp.directives', [])

  .directive('preguntaCard', function() {
    return {
      restric: 'A',
      scope: {
        pregunta: '='
      },
      templateUrl: 'templates/directives/pregunta_estadistica.html',
      controller: function($scope) {
        //console.log($scope.pregunta);
      }
    };
  })