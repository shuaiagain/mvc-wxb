$(function () {

    var _isLogin = $('input[name=isLogin]').val(),
        _loginHeadPartialUrl = $('input[name="loginHeadPartialUrl"]').val(),
        _accountPartialUrl = $('input[name="accountPartialUrl"]').val();

    if (_isLogin) {
        //获取登录后头部html
        $.get(_loginHeadPartialUrl + '?t' + new Date().getTime(), function (backHtml) {

            if (backHtml.indexOf("div") != -1) {
                $(".topcon div.login").html(backHtml);
            }

            $('.info-name').hover(function () {

                $('.user-info').css('background-color', '#5d5a5a');
                $('.info-account').show();
            }, function () {

                $('.user-info').css('background-color', '#000');
                $('.info-account').hide();
            });
        });
    } else {
        login();
    }

    //获取应急状态
    getResponseStatus();

    //获取当前登录者值班信息
    getUserDutyStatus();

    $('.topBar .con-notice').on('click', function (e) {
        $('.home').removeClass('topBar-active');
        $('.record').removeClass('topBar-active');

        $('.home').addClass('topBar-active');
    });

    $('.topBar .record li').on('click', function (e) {
        $('.home').removeClass('topBar-active');
        $('.record').removeClass('topBar-active');

        $('.record').addClass('topBar-active');
    });

    // 列表界面'登录'
    $('.denglu').on('click', function () {
        login();
    });

    //记录
    $('.record').hover(function () {
        $('.record-type').show();
    }, function () {
        $('.record-type').hide();
    });

    var _isPunchinPostBack = true;
    //上班
    $('.icon-punchin').on('click', function (e) {

        if (!_isPunchinPostBack) return false;
        _isPunchinPostBack = false;
        var addDutyLogUrl = $('input[name="addDutyLogUrl"]').val();
        $.post(addDutyLogUrl, {}, function (data) {

            _isPunchinPostBack = true;
            if (data.Code > 0) {
                getUserDutyStatus();
            } else {
                alert(data.Msg);
            }
        });
    });

    var _isPunchoutPostBack = true;
    //下班
    $('.icon-punchout').on('click', function (e) {

        if (!_isPunchoutPostBack) return false;
        _isPunchoutPostBack = false;

        var updateDutyStatusUrl = $('input[name="updateDutyStatusUrl"]').val();
        $.post(updateDutyStatusUrl, { ID: $('.punch').data('dutyid') }, function (data) {

            _isPunchoutPostBack = true;
            if (data.Code > 0) {
                getUserDutyStatus();
            } else {
                alert(data.Msg);
            }
        });
    });

    //点击'响应状态'
    $('.icon-gear').on('click', function (e) {

        var responseId = $('.response-val').data('id');
        responseId = responseId ? responseId : 0;
        var editResponseUrl = $('input[name="editResponseUrl"]').val() + '?responseId=' + responseId;
        $.get(editResponseUrl, function (backHtml) {

            var options = {
                title: false,
                type: 1,
                skin: 'response-show',
                area: ['360px', '200px'],
                closeBtn: false,
                shade: 0.3,
                offset: ['100px'],
                move: false,
                content: backHtml,
                btn: ['保 存', '取 消', '重 置'],
                yes: function (index, layero) {

                    var saveResponseUrl = $('input[name="saveResponseUrl"]').val();
                    var rpParams = {
                        ID: $('.response-val').data('id'),
                        Status: $('input[name="status"]:checked').val(),
                        StartTime: $('.startDate').val(),
                        EndTime: $('.endDate').val()
                    };

                    if (rpParams.StartTime && rpParams.EndTime && rpParams.StartTime > rpParams.EndTime) {
                        alert('响应开始日期不能大于响应结束日期');
                        return false;
                    }

                    $.post(saveResponseUrl, rpParams, function (data) {

                        if (data.Code == 200) {

                            $('.response-val').data('id', data.Data.ID);
                            $('.response-val').data('status', data.Data.Status);
                            $('.response-val').text(data.Data.StatusText);

                            layer.close(index);
                        } else {
                            alert(data.Msg);
                        }
                    });
                },
                btn2: function (index, layero) {

                    layer.close(index);
                },
                btn3: function (index, layero) {
                    //重置
                    $('input[name="status"]').prop('checked', false);
                    $('.startDate,.endDate').val('');
                },
                success: function (layero, index) {
                    //按钮居中
                    layero.find('.layui-layer-btn').css('text-align', 'center');
                }
            };

            layer.open(options);
        });
    });

    //登录
    function login() {

        $.get(_accountPartialUrl, function (backHtml) {

            layer.open({
                title: false,
                type: 1,
                skin: 'login-layer',
                area: ['400px', '260px'],
                closeBtn: false,
                shade: 0.3,
                move: false,
                offset: ['100px'],
                content: backHtml,
                btn: ['登 录'],
                yes: function (index, layero) {

                    hideTip();
                    var user = vaildUser();
                    if (!user) return;

                    //showTip("登入中...");
                    var loginUrl = $('input[name=loginUrl]').val();
                    $.post(loginUrl, user, function (data) {

                        if (data.Code == 200) {
                            layer.close(index);
                            location.href = '/';

                        } else {
                            showTip(data.Msg);
                        }
                    });
                },
                success: function (layero, index) {

                    //按钮居中
                    layero.find('.layui-layer-btn').css('text-align', 'center');

                    //关闭登录弹窗
                    $('.closeBtn').on('click', function () {
                        layer.close(index);
                    });

                    $(document).on("keydown", function (e) {

                        if (e.keyCode == 13) {
                            $(".layui-layer-btn0", '.login-layer').click();
                        }
                    });
                }
            });

            //判断用户输入信息
            function vaildUser() {

                var userName = $.trim($("#lg-userName").val()),
                 password = $.trim($("#lg-password").val());

                if (userName == '') {
                    showTip('用户名不能为空');
                    return false;
                }

                if (password == '') {
                    showTip('密码不能为空');
                    return false;
                }

                return {
                    userName: userName,
                    password: password
                    //isRememberPwd: !!$("#lg-rememberMe:checked")[0]
                };
            }

            function showTip(msg) {
                $("#lg-error").show().text(msg);
            }

            function hideTip() {
                $("#lg-error").hide();
            }
        });
    }

    //获取当前登录者值班信息
    function getUserDutyStatus() {

        $.ajax({
            url: $('input[name="getUserDutyLogUrl"]').val(),
            dataType: 'json',
            type: 'get',
            success: function (data) {

                var punchIn = $('.icon-punchin,.punchin-text');
                var punchOut = $('.icon-punchout,.punchout-text');

                if (data.Code == -200) {

                    punchOut.hide();
                    $('.onduty-names').text('--');
                    $('.onduty-names').attr('title', '--');
                    punchIn.show();
                    return;
                }

                var result = data.Data,
                    ondutyNames = $('.onduty-names'),
                    sumNames = '';

                if (data.Code == 200) {

                    punchIn.hide();
                    punchOut.show();

                    ondutyNames.text(result.CurrentDutyUser);
                    $('.punch').data('dutyid', result.CurrentDutyID);

                    if (result.OtherDutyUser) {
                        for (var i = 0; i < result.OtherDutyUser.length; i++) {

                            sumNames += ' ' + result.OtherDutyUser[i].UserName;
                        }

                        sumNames = ondutyNames.text() + sumNames;
                        ondutyNames.text(sumNames);
                        ondutyNames.attr('title', sumNames);
                    }

                } else if (data.Code == 201) {

                    punchIn.show();
                    punchOut.hide();
                    for (var i = 0; i < result.OtherDutyUser.length; i++) {

                        sumNames += ' ' + result.OtherDutyUser[i].UserName;
                    }

                    ondutyNames.text(sumNames.substr(1));
                    ondutyNames.attr('title', sumNames.substr(1));
                }

            }
        });
    }

    //首页加载获取响应状态
    function getResponseStatus() {

        $.ajax({
            url: $('input[name="getResponseStatusUrl"]').val(),
            dataType: 'json',
            type: 'get',
            success: function (data) {

                var response = $('.response-val');
                if (data.Code > 0) {
                    response.text(data.Data.StatusText);
                    response.attr('data-status', data.Data.Status);
                    response.attr('data-id', data.Data.ID);
                } else {
                    response.text('--');
                    response.attr('data-status', '');
                }
            }
        });
    }

});
