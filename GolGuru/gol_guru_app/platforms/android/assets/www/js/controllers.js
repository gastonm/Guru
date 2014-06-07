angular.module('GolGuruApp.controllers', [])



.controller('LogInCtrl', function($scope, $state) {
    $scope.showAntesPartido = function() {
      $state.go('antes_partido');

      //When user logs in, delete localStorage.
      localStorage.clear();
    };
})

.controller('EstadisticaCtrl', function($scope, $timeout, preguntas_estadisticas, tiempo_jugadores, Projects) {
  $scope.preguntas_estadisticas = preguntas_estadisticas;
  $scope.totalPreguntas = angular.fromJson(preguntas_estadisticas[0]).lenght;
  $scope.preguntaSegundos = tiempo_jugadores[0]['segundos_para_partido'];
  $scope.totalJugadores = tiempo_jugadores[0]['total_jugadores'];

  // Load or initialize projects
  $scope.projects = Projects.all();

  //In HTML print $scope.preguntaSegundosShow for proper time format
  $scope.preguntaSegundosShow;
  var mytimeout;
  
  //ERROR: Cada vez que vuelvo de pregunta a estadistica el reloj se resetea
  $scope.onTimeout = function(){
      var preguntaMinutosLocal, preguntaSegundosLocal;
      //If time is less than 60 sec, format is SEC
      if($scope.preguntaSegundos < 60) {
        $scope.preguntaSegundosShow = $scope.preguntaSegundos;
      }
      //If time is more than 60 sec, change format to MIN : SEC
      else{
        preguntaMinutosLocal = Math.floor( $scope.preguntaSegundos / 60 );
        preguntaSegundosLocal = ($scope.preguntaSegundos)%60;
        if(preguntaSegundosLocal<10) {
          preguntaSegundosLocal = "0" + preguntaSegundosLocal;
        }
        $scope.preguntaSegundosShow = preguntaMinutosLocal + " : " + preguntaSegundosLocal;
      }
      if($scope.preguntaSegundos == 0) {
        //Hide all modals
        //$scope.entretiempo.hide();
        //$scope.figura.hide();
        //$scope.ultimoGol.hide();
      }
      //Wait 5 seconds after countdown and GET the correct answer from server
      else if($scope.preguntaSegundos == -5) {
        $scope.openModalResultado();
      }
      //Show resultado modal for 10 seconds and then hide
      else if($scope.preguntaSegundos == -15) {
        $scope.closeModalResultado();
      }
      
      mytimeout = $timeout($scope.onTimeout,1000);
      $scope.preguntaSegundos--;
      //console.log($scope.preguntaSegundos);
  }

  //Start the coundown
  var mytimeout = $timeout($scope.onTimeout,1000);

  //Function for creating projects
  var createProject = function(projectTitle, respuesta) {
    var newProject = Projects.newProject(projectTitle, respuesta);
    $scope.projects.push(newProject);
    Projects.save($scope.projects);
    //alert("create");
  }
  //Create project for all the questions before match if there is nothing in local storage.
  //First time you acces to all the question will create the elements, then will do nothing
  //send i+1 because thats the ID of the question
  if(!window.localStorage['projects']) {
    for(var i=0; i<$scope.preguntas_estadisticas.length; i++) {
      //Create element with emty anser, then is updated in PreguntaEstadisticaOpenCtrl
      createProject(i+1,"");
    }
  }

})

.controller('AntesPartidoCtrl', function($scope, $ionicPlatform, $ionicScrollDelegate, $ionicModal, $state, $http, $timeout, Projects) {
  //Links
  $scope.goToEstadistica = function() {
    $state.go('estadistica')
  }
  $scope.goToPosiciones = function() {
    $state.go('posiciones')
  }

  //Declare scope variables. Add value after geting data from server.
  $scope.preguntaID;
  $scope.preguntaTipo;
  $scope.preguntaPregunta;
  $scope.preguntaRespuestas;
  $scope.imageShow;
  //$scope.preguntaSegundos;

  //Declare scope variables. Add value after geting data from server.
  $scope.respuestaCorrectaResultado;
  $scope.fraceResultado;
  $scope.puntosResultado;

  //Countdown (updates value of preguntaSegundos every second)
  $scope.preguntaSegundos;
  //In HTML print $scope.preguntaSegundosShow for proper time format
  $scope.preguntaSegundosShow;
  var mytimeout;
  
  $scope.onTimeout = function(){
      var preguntaMinutosLocal, preguntaSegundosLocal;
      //If time is less than 60 sec, format is SEC
      if($scope.preguntaSegundos < 60) {
        console.log('<60');
        $scope.preguntaSegundosShow = $scope.preguntaSegundos;
      }
      //If time is more than 60 sec, change format to MIN : SEC
      else{
        preguntaMinutosLocal = Math.floor( $scope.preguntaSegundos / 60 );
        preguntaSegundosLocal = ($scope.preguntaSegundos)%60;
        if(preguntaSegundosLocal<10) {
          preguntaSegundosLocal = "0" + preguntaSegundosLocal;
        }
        $scope.preguntaSegundosShow = preguntaMinutosLocal + " : " + preguntaSegundosLocal;
      }
      //If time is 0, close modal
      if($scope.preguntaSegundos == 0) {
        //Close allQuestions modal
        $scope.closeModalAllQuestions('');
      }
      //Wait 5 seconds after countdown and GET the correct answer from server
      else if($scope.preguntaSegundos == -5) {
        $scope.openModalResultado();
      }
      //Show resultado modal for 10 seconds and then hide
      else if($scope.preguntaSegundos == -15) {
        $scope.closeModalResultado();
        $scope.allQuestions.remove();
      }
      //Run function every 1 second
      mytimeout = $timeout($scope.onTimeout,1000);
      $scope.preguntaSegundos--;
      //console.log($scope.preguntaSegundos);
  }

  //Doens't stop the countdown
  $scope.stop = function(){
      $timeout.cancel(mytimeout);
  }

  //Start the coundown
  var mytimeout;

  // Load or initialize projects
  $scope.projects = Projects.all();


  //Add the answer to localstorage projects,
  //call this fucntion when closing modal
  var createProject = function(projectTitle, respuesta) {
    var newProject = Projects.newProject(projectTitle, respuesta);
    $scope.projects.push(newProject);
    Projects.save($scope.projects);
    console.log(JSON.stringify($scope.projects));
  }

  /*********** MODAL: ALL QUESTIONS ***********/
  //templateUrl loads the template url and
  //thisJson gets the data from the proper json file
  $scope.createModal = function(templateUrl, thisJson){
    $ionicModal.fromTemplateUrl('templates/'+templateUrl+'.html', {
      scope: $scope,
      animation: 'slide-in-up'
    }).then(function(modal) {
      $scope.allQuestions = modal;
    });
    $scope.openModalAllQuestions(thisJson);
  }
  //Send the json name of the file that must be load as a parameter
  $scope.openModalAllQuestions = function(thisJson) {
    //Stop the countdown and start again.
    $scope.stop();
    //Scroll to the top of the modal (not usign becasue create a new modal every time we open a question)
    //$ionicScrollDelegate.scrollTop(true);
    $http.get('js/JSON/' + thisJson + '.json').then(function(response) {
            $scope.preguntaID = response.data[0]['id'];
            $scope.preguntaTipo = response.data[0]['tipo'];
            $scope.preguntaPregunta = response.data[0]['pregunta'];
            $scope.preguntaSegundos = response.data[0]['segundos'];
            $scope.preguntaRespuestas = response.data[0]['respuestas'];
            //Show modal
            $scope.allQuestions.show();
            //Start the coundown inmediately after the modal is shown
            mytimeout = $timeout($scope.onTimeout,0);
    })
  };
  $scope.closeModalAllQuestions = function(thisAnswer) {
    createProject($scope.preguntaID, thisAnswer);
    //Remove modal after answering the question because we create a new modal every time we open a question
    $scope.allQuestions.remove();
    //$scope.allQuestions.hide();
  };
  //Create a project element when user press back btn, same as cancel
  document.addEventListener("backbutton", function(){
    createProject($scope.preguntaID, '');
    $scope.allQuestions.remove();
  }, false);

  /*********** MODAL: RESULTADO ***********/
  $ionicModal.fromTemplateUrl('templates/resultado.html', {
    scope: $scope,
    animation: 'slide-in-up'
  }).then(function(modal) {
    $scope.resultado = modal;
  });
  $scope.openModalResultado = function() {
    $scope.resultado.show();

    //Start the coundown
    //var mytimeout = $timeout($scope.onTimeout,1000);

    $http.get('js/JSON/resultado.json').then(function(response) {
            //Use JSON.stringify for compare to respuesta
            $scope.respuestaCorrectaResultado = JSON.stringify(response.data[0]['respuesta_correcta']);
            var fracePositiva = response.data[0]['frace_positiva'];
            var fraceNegativa = response.data[0]['frace_negativa'];
            var puntos = response.data[0]['puntos'];
            $scope.porcentajeAcertaronResultado = response.data[0]['porcentaje_acertaron'];
            //Answer made by the user
            //IMPORTANT: $scope.preguntaID must have te value of the question we want to validate. That question has to be the last seen by the user
            var respuesta = JSON.stringify($scope.projects[$scope.preguntaID-1]['respuesta']);
            if(respuesta == $scope.respuestaCorrectaResultado){
              $scope.fraceResultado = fracePositiva;
              $scope.puntosResultado = "Sumaste " + puntos + " puntos!";
            } else {
              $scope.fraceResultado = fraceNegativa;
              $scope.puntosResultado = "Respuesta incorrecta!";
            }

            console.log(respuesta + "=" + $scope.respuestaCorrectaResultado);
    })

    if($scope.preguntaSegundos == 0){
      $scope.resultado.hide();
    }
  };
  $scope.closeModalResultado = function() {
    $scope.resultado.hide();
  };
  //Cleanup the modal when we're done with it!
  $scope.$on('$destroy', function() {
    $scope.resultado.remove();
  });

})

.controller('PreguntaEstadisticaOpenCtrl', function($scope, $location, Projects) {
    $scope.location = $location;
    $scope.$watch('location.search()', function() {
        //Declare parameters that are being passed in URL
        $scope.id = ($location.search()).id;
        $scope.pregunta = ($location.search()).pregunta;
        $scope.respuestas = ($location.search()).respuestas;
        //IMPORTANT: I'm getting a String from URL, here I convert it into JSON object
        $scope.respuestas = angular.fromJson($scope.respuestas);

        //Save the value of the current answer for the $scope.id
        //for adding class selected in html (doing it initialicing creating $scope.id)
        var idInProjects = $scope.id-1;
        if($scope.projects[idInProjects]['respuesta']) {
          $scope.selected = $scope.projects[idInProjects]['respuesta'];
        }

    }, true);


    // A utility function for creating a new project
    // with the given projectTitle and respuesta
    var createProject = function(projectTitle, respuesta) {
      var newProject = Projects.newProject(projectTitle, respuesta);
      $scope.projects.push(newProject);
      Projects.save($scope.projects);
      //alert("create");
    }

    // A utility function for uptadint an existing project
    // with the given projectTitle, respuesta and index in the projects object
    var updateProject = function(projectTitle, respuesta, i) {
      var newProject = Projects.newProject(projectTitle, respuesta);
      $scope.projects.splice(i, 1, newProject);
      Projects.save($scope.projects);
      //alert("update");
    }

    // Load or initialize projects
    $scope.projects = Projects.all();

    //Save the latest rasnwer in the localstorage
    $scope.saveAnswer = function(respuesta) {
      //Save the scope id in a variable
      var id = $scope.id;
      //Get the lenght of projects
      var numeroPreguntas = $scope.projects.length;
      var resultado = '';
      //If projects empty, creat new respuesta
      if(numeroPreguntas == 0) createProject(id, respuesta);
      //Else...
      else {
        var total = $scope.projects;
        for(var i=0;i<numeroPreguntas;i++) {
          if ($scope.projects[i]['id']){
            var forID = $scope.projects[i]['id'];
            //If there is an answer for that question, set reultado to true and exit loop
            if(forID == id) {
              resultado = 'true';
              break;
            //If there isn't an answer for that question, set reultado to false and keep in the loop
            } else {
              resultado = 'false';
            }
          }
        }
        //Check value of resultado and creat or update respuesta
        if(resultado == 'false') {
          //UpdateProject instead of create project because the project is creted in EstadisticaCtrl when open for the first time
          updateProject(id, respuesta, i);
          //createProject(id, respuesta);
        } else if(resultado == 'true') {
          updateProject(id, respuesta, i);
        }
      }
      console.log(JSON.stringify($scope.projects));
      return resultado;
    };
})

.controller('PosicionesCtrl', function($scope, posiciones) {
  $scope.puntos = posiciones[0]['puntos'];
  $scope.puesto = posiciones[0]['puesto'];
  $scope.total_jugadores = posiciones[0]['total_jugadores'];
  $scope.posiciones = posiciones[0]['posiciones'];
})







