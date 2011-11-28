var model = {
    x: 0,		        //Ball Position x
    y: 0,	            //Ball Position y
    player1Score: 0,	//Player1 Score
    player2Score: 0,    //Player2 Score
    speed   : 15,       //Render time out
    gameStopped: true,  //Global flag if game is running or currently stopped
    vx: 2,              //new position vector x
    vy: 2,              //new position vector y
    v: 2,               //new position multiplier   
    clientId: "guid",   //Signal Connection Client Guid
    position: -1,       //paddle position (0==Left, 1==Right)
    action: "wait"      //Toggles the wait for player screen
}

//updates the field, scores and the ball
function update(model) {

    var ball = $("#ball"),
        paddle = $("#paddle"),
        playfield = $("#playfield"),
        pfWidth = parseInt(playfield.css('width')),
        pfHeight = parseInt(playfield.css('height')),
        bWidth = parseInt(ball.css('width')),
        lpLeft = parseInt(paddle.css('left')),
        lpWidth = parseInt(paddle.css('width')),
        lpTop = parseInt(paddle.css('top')),
        lpHeight = parseInt(paddle.css('height'));

    $("#score").html("Player1: " + model.player1Score + " / Player2: " + model.player2Score);
   
    if (!model.gameStopped) {

        //field validation - bottom
        if (model.y + bWidth >= pfHeight) {
            model.vy = Math.cos(135) * model.v;
        };

        //field validation - top
        if (model.y <= 0) {
            model.vy = Math.cos(45) * model.v;
        };

        if (model.position==0) {
            //left paddle
            //pass the ball to the player on the right
            if (model.x + bWidth >= pfWidth) {
                model.gameStopped = true;
                $("#ball").hide();
                connection.send("spawn=right,id=" + model.clientId + ",x=0,y=" + model.y + ",vy=" + model.vy + ",vx=" + model.vx);
            };
            //left player lost
            if (model.x < 0) {
                model.gameStopped = true;
                $("#ball").hide();
                connection.send("score=" + model.clientId);
            };
        } else {
            //right paddle
            //pass the ball to the player on the left
            if (model.x < 0) {
                model.gameStopped = true;
                $("#ball").hide();
                connection.send("spawn=left,id=" + model.clientId + ",x=" + (pfWidth - bWidth - 1) + ",y=" + model.y + ",vx=" + model.vx + ",vy=" + model.vy);
            };
            //right player lost
            if (model.x + bWidth >= pfWidth) {
                model.gameStopped = true;
                $("#ball").hide();
                connection.send("score=" + model.clientId);
            };
        }
    
      if (model.position == 0) {
            //ball returned by the left paddle
            if ((model.x <= lpLeft + lpWidth) && (model.y >= lpTop) && (model.y <= lpTop + lpHeight)) {
                    model.vx = Math.cos(45) * model.v;
            }
      } else {
            //ball returned by the right paddle
            if ((model.x + bWidth >= lpLeft) && (model.y >= lpTop) && (model.y <= lpTop + lpHeight)) {
                model.vx = Math.cos(135) * model.v;
            }
      }
        
      model.x = model.x + model.vx * model.v;
      model.y = model.y + model.vy * model.v;
        
      ball.css('top', model.y);
      ball.css('left', model.x);
      setTimeout(function () { update(model) }, model.speed);
   
    }
    
}

//respawns the ball on the field if passed over from the other player
function respawn(pos) {
    
    if (pos == model.position) {
        var paddle = $("#paddle"),
            ball = $("#ball"),
            pl = parseInt(paddle.css("left")),
            ph = parseInt(paddle.css("height")),
            pt = parseInt(paddle.css("top")),
            pw = parseInt(paddle.css("width")),
            bw = parseInt(ball.css("width"));

        ball.css("top", pt + ph / 2);
    
        if (pos==0) {
            model.x = pl + pw;
        }else {
            model.x = pl - bw;
        }

        ball.show();
        model.gameStopped = false;
        update(model);
    }
    
}

function Start() {

    $("#ball").hide();
    InitConnection();
    
}


// -- SignalR --
// onnection initialization and recieved callback registration
var clientId = null;
var connection = null;

function InitConnection() {
 
    connection = $.connection('echo');
    connection.received(function (data) {

        var command = $.parseJSON(data.toString());

        if (command != null) {

            switch (command.cmd) {

                case "init":
                    //There is player registered
                    if (model.clientId == "guid") {
                        model.clientId = command.clientId;
                        model.position = command.position;
                    }
                    model.action = command.action;
                    if (model.action === 'go') {
                        $('#welcome').hide();
                        $('#playfield').show();
                        if (model.gameStopped) {
                            model.gameStopped = false;
                            model.playerScore = -1;

                            if (model.position == 0) {
                                $("#paddle").css('left', 15);
                            } else {
                                $("#paddle").css('left', 270);
                            }

                            //Touch support initialization
                            $('#paddle').touch({
                                animate: false,
                                sticky: false,
                                dragx: false,
                                dragy: true,
                                rotate: false,
                                resort: true,
                                scale: false
                            });

                            //The player on the left starts the game
                            if (model.position == 0) {
                                $('#ball').show();
                                update(model);
                            }
                        }
                    };

                    break;

                case "respawn":
                    //we have the ball again, start the game loop again
                    if (command.clientId === model.clientId) {
                        model.x = command.X;
                        model.y = command.Y;
                        model.vy = command.VY;
                        model.vx = command.VX;
                        $("#ball").show();
                        model.gameStopped = false;
                        setTimeout(function () { update(model) }, model.speed);
                    }

                    break;
                    
                case "newscore":
                    //update the scores
                    if (command.position == 0) {
                        model.player1Score = command.score;
                    } else {
                        model.player2Score = command.score;
                    };

                    respawn(command.position);

                    break;
            }
        }
    });
    
    // Send a hello to the server
    connection.start("", function () {
        connection.send("hello");
    });
};



    