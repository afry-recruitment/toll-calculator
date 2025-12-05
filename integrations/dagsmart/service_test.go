package dagsmart

import (
	"errors"
	"io"
	"net/http"
	"reflect"
	"strings"
	"testing"
	"testing/iotest"

	mock_dagsmart "afry-toll-calculator/mocks/afry-toll-calculator/integrations/dagsmart"
	"github.com/stretchr/testify/mock"
)

func Test_svc_Get(t *testing.T) {
	validAPIResponse := &http.Response{
		Body: io.NopCloser(strings.NewReader(`[
			{"date":"2013-01-01","code":"newYearsDay","name":{"en":"New Year's Day","sv":"nyårsdagen"}},
			{"date":"2013-01-06","code":"epiphany","name":{"en":"Epiphany","sv":"trettondedag jul"}}
		]`)),
	}
	validAPIResponseItems := []string{"2013-01-01", "2013-01-06"}

	tests := []struct {
		name        string
		year        int
		mocks       func(*mock_dagsmart.MockHttpGetter)
		want        []string
		wantErr     bool
		wantErrText string
	}{
		{
			name: "API error",
			mocks: func(getter *mock_dagsmart.MockHttpGetter) {
				getter.EXPECT().
					Get(mock.Anything).
					Return(nil, errors.New("foo"))
			},
			wantErr:     true,
			wantErrText: "foo",
		},
		{
			name: "API response bad data",
			mocks: func(getter *mock_dagsmart.MockHttpGetter) {
				getter.EXPECT().
					Get(mock.Anything).
					Return(&http.Response{
						Body: io.NopCloser(strings.NewReader("foo-data")),
					}, nil)
			},
			wantErr:     true,
			wantErrText: "failed to unmarshal JSON response",
		},
		{
			name: "API response date format validation error",
			mocks: func(getter *mock_dagsmart.MockHttpGetter) {
				getter.EXPECT().
					Get(mock.Anything).
					Return(&http.Response{
						Body: io.NopCloser(strings.NewReader(`[{"date":"01-01-2013","code":"newYearsDay","name":{"en":"New Year's Day","sv":"nyårsdagen"}}]`)),
					}, nil)
			},
			wantErr:     true,
			wantErrText: "failed to validate item date format",
		},
		{
			name: "API response date format validation error",
			mocks: func(getter *mock_dagsmart.MockHttpGetter) {
				getter.EXPECT().
					Get("https://api.dagsmart.se/holidays?weekends=false&year=1337").
					Return(validAPIResponse, nil)
			},
			year:    1337,
			wantErr: false,
			want:    validAPIResponseItems,
		},
		{
			name: "read body error",
			mocks: func(getter *mock_dagsmart.MockHttpGetter) {
				getter.EXPECT().
					Get(mock.Anything).
					Return(&http.Response{
						Body: io.NopCloser(iotest.ErrReader(errors.New("reader error")))}, nil)
			},
			wantErr:     true,
			wantErrText: "reader error",
		},
	}
	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			httpGetter := mock_dagsmart.NewMockHttpGetter(t)
			tt.mocks(httpGetter)

			s := &svc{
				httpService: httpGetter,
			}

			got, err := s.Get(tt.year)
			if (err != nil) != tt.wantErr {
				t.Errorf("Get() error = %v, wantErr %v", err, tt.wantErr)
				return
			}
			if tt.wantErr && err.Error() != tt.wantErrText {
				t.Errorf("Get() error = %v, wantErrText %v", err, tt.wantErrText)
				return
			}
			if !reflect.DeepEqual(got, tt.want) {
				t.Errorf("Get() got = %v, want %v", got, tt.want)
			}
		})
	}
}
